# filereader-library
A file reader library used for reading multiple types of files.

### Current version: version 10.0

## Contents

This library can be used to read a selected type of files. Currently the supported file types are: TEXT (.txt), XML (.xml) and JSON (.json).

### FileReaderManager

The FileReaderManager is used to retrieve the instance of the FileReader. This instance is handled by the FileReaderManager. Using the manager certain funtionality of the FileReader can be overridden.

- FileSystem: by implementing your own version of the `IFileSystem` interface you can use a diffrent source for your files. Make sure `File.Exists()` and `File.Open.Read()` are implemented. Use the `SetFileSystem(IFileSystem filesystem)` method to override the default implementation.

- EncryptionHandler: by implementing your own version of the `IEncryptionHandler` interface you can use a diffrent algorithm to decrypt the file you are reading. Use the `SetEncryptionHandler(IEncryptionHandler encryptionHandler)` method to override the default implementation.

- PermissionHandler: by implementing your own version of the `IPermissionsHandler` interface you can use a diffrent algorithm to handle role base security on the file you want to read. Use the `SetPermissionsHanldler(IPermissionsHandler permissionsHandler)` method to override the default implementation.

### IFileReader

The `IFileReader` instance is used to read the actual files. The following methods are currently available:
- `ReadTextFile(FileReadRequest request)` is used for reading text(.txt) files.
- `ReadXmlFile(FileReadRequest request)` is used for reading xml(.xml) files.
- `ReadJsonFile(FileReadRequest request)` is used for reading json(.json) files.