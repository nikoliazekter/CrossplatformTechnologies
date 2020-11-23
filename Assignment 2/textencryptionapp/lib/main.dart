import 'package:flutter/material.dart';
import 'package:file_picker/file_picker.dart';
import 'package:permission_handler/permission_handler.dart';
import 'encrypter.dart';
import 'dart:typed_data';
import 'dart:convert';
import 'dart:io';
import 'package:universal_html/html.dart' as html;
import 'package:flutter/foundation.dart' show kIsWeb;

void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Text Encryption App',
      theme: ThemeData(
        primaryColor: Colors.white,
      ),
      home: MainMenu(),
    );
  }
}

class MainMenu extends StatefulWidget {
  @override
  _MainMenuState createState() => _MainMenuState();
}

class _MainMenuState extends State<MainMenu> {
  final TextStyle myStyle = TextStyle(
    fontSize: 20.0,
  );

  String _encryptionAlgorithm = 'ShiftRows';
  bool _shouldEncrypt = true;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: DefaultTabController(
        length: 3,
        child: Scaffold(
          appBar: AppBar(
            title: Text('Text Encryption App'),
            bottom: TabBar(
              isScrollable: true,
              tabs: [
                Tab(text: 'ENCRYPT/DECRYPT'),
                Tab(text: 'USER GUIDE'),
                Tab(text: 'ABOUT'),
              ],
            ),
          ),
          body: TabBarView(
            children: [
              _buildEncryptDecryptTab(),
              _buildUserGuideTab(),
              _buildAboutTab(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildEncryptDecryptTab() {
    return Column(crossAxisAlignment: CrossAxisAlignment.center, children: [
      Container(
        padding: EdgeInsets.all(20),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.end,
          children: [
            Container(
              padding: EdgeInsets.only(bottom: 10, right: 30),
              child: Text(
                'Encryption algorithm:',
                style: myStyle,
              ),
            ),
            DropdownButton(
                hint: Text('Choose'),
                value: _encryptionAlgorithm,
                items: <DropdownMenuItem>[
                  DropdownMenuItem(
                    child: Text(
                      'ShiftRows',
                      style: myStyle,
                    ),
                    value: 'ShiftRows',
                  ),
                  DropdownMenuItem(
                    child: Text(
                      'SubBytes',
                      style: myStyle,
                    ),
                    value: 'SubBytes',
                  ),
                  DropdownMenuItem(
                    child: Text(
                      'MixColumns',
                      style: myStyle,
                    ),
                    value: 'MixColumns',
                  ),
                ],
                onChanged: (value) => setState(() {
                      _encryptionAlgorithm = value;
                    })),
          ],
        ),
      ),
      Container(
        padding: EdgeInsets.all(20),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Container(
              padding: EdgeInsets.only(top: 15, right: 20),
              child: Text(
                'Action:',
                style: myStyle,
              ),
            ),
            Expanded(
              child: Column(
                children: [
                  ListTile(
                    title: Text(
                      'Encrypt',
                      style: myStyle,
                    ),
                    leading: Radio(
                      value: true,
                      groupValue: _shouldEncrypt,
                      onChanged: (bool value) {
                        setState(() {
                          _shouldEncrypt = value;
                        });
                      },
                    ),
                  ),
                  ListTile(
                    title: Text(
                      'Decrypt',
                      style: myStyle,
                    ),
                    leading: Radio(
                      value: false,
                      groupValue: _shouldEncrypt,
                      onChanged: (bool value) {
                        setState(() {
                          _shouldEncrypt = value;
                        });
                      },
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
      Container(
        padding: EdgeInsets.only(top: 50),
        child: ElevatedButton(
          child: Text(
            'Pick text file',
            style: myStyle,
          ),
          onPressed: _encryptionAlgorithm != null ? _pickTextFileEncrypt : null,
        ),
      ),
    ]);
  }

  Widget _buildUserGuideTab() {
    String userGuideText = '''
Encryption:
  1. Go to ENCRYPT/DECRYPT tab.
  2. Choose desired encryption algorithm.
  3. Choose Encrypt action.
  4. Tap 'Pick text file' button to open file explorer
  5. Navigate to text file you want to encrypt and tap on it.
  6. The results page will show up on the screen with original and encrypted text.
  7. If you wish to save encrypted file tap Save in top right corner and choose desired folder.
  8. Tap on arrow in top left corner to go back to main menu.
  
Decryption:
  1. Go to ENCRYPT/DECRYPT tab.
  2. Choose encryption algorithm which was used to encrypt the file. All other options will produce garbage.
  3. Choose Decrypt action.
  4. Follow steps 4-8 in encryption guide.

''';
    return Container(
      padding: EdgeInsets.all(20),
      child: Text(
        userGuideText,
        style: TextStyle(
          fontSize: 18,
        ),
      ),
    );
  }

  Widget _buildAboutTab() {
    String aboutText = '''
Application that allows users to encrypt and decrypt their text files using three different encryption methods.



Developed as part of Cross-platform and Multi-platform Technologies course at Taras Shevchenko National University of Kyiv.

Author: Mykola Zekter, nikoliazekter@gmail.com

All rights reserved ©''';
    return Container(
      padding: EdgeInsets.all(20),
      child: Text(
        aboutText,
        style: myStyle,
      ),
    );
  }

  void _pickTextFileEncrypt() async {
    FilePickerResult result = await FilePicker.platform.pickFiles(
        type: FileType.custom, allowedExtensions: ['txt'], withData: true);

    if (result != null) {
      PlatformFile file = result.files.first;
      Encrypter encrypter = _getEncrypter();
      Uint8List oldBytes = file.bytes;
      Uint8List newBytes;
      if (_shouldEncrypt)
        newBytes = encrypter.encrypt(oldBytes);
      else
        newBytes = encrypter.decrypt(oldBytes);
      String newName = '${_encryptionAlgorithm}_' +
          (_shouldEncrypt ? 'enc' : 'dec') +
          '_${file.name.substring(0, file.name.length - 4)}.txt';
      Navigator.push(
        context,
        MaterialPageRoute(
            builder: (context) => ResultsScreen(newName, oldBytes, newBytes)),
      );
    } else {
      // User canceled the picker
    }
  }

  Encrypter _getEncrypter() {
    switch (_encryptionAlgorithm) {
      case 'ShiftRows':
        return ShiftRowsEncrypter();

      case 'SubBytes':
        return SubBytesEncrypter();

      case 'MixColumns':
        return MixColumnsEncrypter();
    }
  }
}

class ResultsScreen extends StatelessWidget {
  final String filename;
  final Uint8List originalBytes;
  final Uint8List newBytes;
  final String originalText;
  final String newText;

  ResultsScreen(this.filename, this.originalBytes, this.newBytes)
      : originalText = utf8.decode(originalBytes, allowMalformed: true),
        newText = utf8.decode(newBytes, allowMalformed: true);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Results'), actions: [
        TextButton(
          child: Text('Save'),
          onPressed: _saveResult,
        ),
      ]),
      body: Row(
        mainAxisAlignment: MainAxisAlignment.spaceEvenly,
        children: [
          Expanded(
            child: Column(
              children: [
                Container(
                  padding: EdgeInsets.all(20),
                  child: Text(
                    'Original',
                    style: TextStyle(
                      fontSize: 20,
                    ),
                  ),
                ),
                Expanded(
                  child: Scrollbar(
                    child: SingleChildScrollView(
                      child: Container(
                        margin: EdgeInsets.all(5),
                        padding: EdgeInsets.all(5),
                        decoration: BoxDecoration(
                          border: Border.all(color: Colors.grey),
                        ),
                        child: Text(originalText),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
          Expanded(
            child: Column(
              children: [
                Container(
                  padding: EdgeInsets.all(20),
                  child: Text(
                    'New',
                    style: TextStyle(
                      fontSize: 20,
                    ),
                  ),
                ),
                Expanded(
                  child: Scrollbar(
                    child: SingleChildScrollView(
                      child: Container(
                        margin: EdgeInsets.all(5),
                        padding: EdgeInsets.all(5),
                        decoration: BoxDecoration(
                          border: Border.all(color: Colors.grey),
                        ),
                        child: Text(newText),
                      ),
                    ),
                  ),
                ),
              ],
            ),
          ),
        ],
      ),
    );
  }

  void _saveResult() async {
    if (kIsWeb) {
      _saveResultWeb();
    } else {
      if (await Permission.storage.request().isGranted) {
        String dirPath = await FilePicker.platform.getDirectoryPath();
        File file = File('$dirPath/$filename.txt');
        file.writeAsBytes(newBytes, flush: true);
      }
    }
  }

  void _saveResultWeb() {
    // prepare
    final blob = html.Blob([newBytes]);
    final url = html.Url.createObjectUrlFromBlob(blob);
    final anchor = html.document.createElement('a') as html.AnchorElement
      ..href = url
      ..style.display = 'none'
      ..download = '$filename';
    html.document.body.children.add(anchor);

    // download
    anchor.click();

    // cleanup
    html.document.body.children.remove(anchor);
    html.Url.revokeObjectUrl(url);
  }
}
