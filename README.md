# Skeletom's Unity Essentials

This is a collection of the same twenty-or-so classes that I continually copy and paste in to basically every new Unity project I end up working on. Made sense to package them together and make this process easier for myself in the future. And now here you are, benefitting from it as well.


## Lifecycle

### `Singleton<T>`
Does what it says on the tin. An abstract class which implements the component as a singleton in the scene.

### `OrderedInitializer`
An abstract class which `Singleton` inherits from. Allows singletons (or other components which exist in the scene) to have their Initialization order clearly defined in a configurable way. 

For example, you may want your UI Manager to set up before you have your Save Manager load data in and populate said UI.

### `ObjectPool<T>`
A class which instantiates and stores objects of type `T`, preventing them from needing to instantiate them at runtime. Useful for things like projectiles or particles, which may need to be summoned in rapid, large bursts.

New objects will be instantiated and added to the pool as needed, if the pool is ever completely exausted at once.

## I/O

### `LoggingManager`
A `Singleton` component which handles formatting and curating log files. Automatically retains up to a configurable number of logs from recent executions, and culls log files that fall beyond that. 

Any calls to Unity's built-in `Debug` logger will pipe to this output, so it slots very nicely in to just about any existing Unity project with no fuss.

### `SaveDataManager`
A `Singleton` component which handles reading and writing files to disk. 

### `ISaveable<T>`
An interface that defines a class as being able to be serialized to disk in some way. The interface requires a generic parameter which extends `BaseSaveData` in order to specify the structure of the serialized representation. 

The `SaveDataManager` has aptly-named `WriteSaveData` and `ReadSaveData` methods that accept an `ISaveable` and handles the serialization and deserialization.

### `BaseSaveData`
An abstract class that defines the JSON structure of an `ISaveable`.

## Localization

### `LocalizationManager`
A `Singleton` component which loads a CSV table in to memory and parses it as a string lookup table, with the firsrt column representing keys and subsequent columns representing the same text in different languages.

### `LocalizedText`
A component which has a `key` property, which it uses to fetch localized text from the `LocalizationManager` when the component is made active. 

All `LocalizedText` components are automatically notified and re-localized when the `LocalizationManager` switches languages.

### `ITextElement`
An interface which wraps a Text UI component, such as the UnityEngine.UI `Text` component or the TMP `TMP_Text` component, which bizarrely do not already share an interface.

This is used by the `LocalizedText` component so that it can easily localize text regardless of how the text is displayed.

### `Editor/CSVPuller`
An editor extension class that allows you to provide the URL of a Google Sheet and automatically download the contents into a target TextAsset as a CSV, for use as a string table with the `LocalizationManager`.

## Utils

### `MathUtils`
A utility class that does what it says on the tin. Useful math functions, wow!

### `EnumUtils`
A utility class for dealing with Enums and other enumerables. Specifically, I always find myself wanting a Wait routine that can be interrupted at any frame, which this provides.

### `HTTPUtils`
A utility class that provides an easy, callback-driven set of methods for making GET and POST requests from Unity.
Leveraged by the later `VersionUtils` class to check a remote server for version updates.

### `VersionUtils`
A utility class which provides an easy, callback-driven method for checking if a new version of the application exists. 

This one's a lifesaver for me, as it allows users of my various toys and tools to be proactively informed when I release updates, without needing some kind of launcher or other platform.