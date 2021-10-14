# filereader-library
A file reader library used for reading multiple types of files.

### Current version: version 11.0
This version not only holds the library, there is also an example WPF application.

## Contents

This library can be used to read a selected type of files. Currently the supported file types are: TEXT (.txt), XML (.xml) and JSON (.json).

### FileReaderManager

The FileReaderManager is used to retrieve the instance of the FileReader. This instance is handled by the FileReaderManager. Using the manager certain funtionality of the FileReader can be overridden.

- FileSystem: by implementing your own version of the `IFileSystem` interface you can use a diffrent source for your files. Make sure `File.Exists()` and `File.Open.Read()` are implemented. Use the `SetFileSystem(IFileSystem filesystem)` method to override the default implementation.

- EncryptionHandler: by implementing your own version of the `IEncryptionHandler` interface you can use a diffrent algorithm to decrypt the file you are reading. Use the `SetEncryptionHandler(IEncryptionHandler encryptionHandler)` method to override the default implementation. Make sure to override the `DecryptFileContent`-method.

- PermissionHandler: by implementing your own version of the `IPermissionsHandler` interface you can use a diffrent algorithm to handle role base security on the file you want to read. Use the `SetPermissionsHanldler(IPermissionsHandler permissionsHandler)` method to override the default implementation. Make sure to override the `HasReadPermission`-method.

### IFileReader

The `IFileReader` instance is used to read the actual files. The following methods are currently available:
- `ReadTextFile(FileReadRequest request)` is used for reading text(.txt) files. The filecontent is returned as a `string`.
- `ReadXmlFile(FileReadRequest request)` is used for reading xml(.xml) files. The filecontent is returned as a `XmlDocument`
- `ReadJsonFile(FileReadRequest request)` is used for reading json(.json) files.The filecontent is returned as a `JsonDocument`
- `ReadFile(FileReadRequest request)` can be used to read any file as long as it is one of the supported file types. The filecontent is returned as one of the above mentioned above, depending on the file type of the file being read.

### FileReadRequest

Each method the IFileReader provides requires a `FileReadRequest` object. It is used as follows:
- Pass the path to the file you want to read in the contructor of the object.
- Set the `UseEncryption` property to true if you want to Decrypt the file to read.
- Set the `UsePermissions` property to true if you want to use Role Base Security.
- Set the `RoleName` property to the role name you want to use for the Role Base Security.


