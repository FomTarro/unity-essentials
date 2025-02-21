# Skeletom's Unity Essentials

This is a collection of the same twenty-or-so classes that I continually copy and paste in to basically every new Unity project I end up working on. Made sense to package them together and make this process easier for myself in the future. And now here you are, benefitting from it as well.


## Lifecycle

### `Singleton<T>`
Does what it says on the tin. An abstract class which implements the component as a singleton in the scene.

### `OrderedInitializer`
An abstract class which `Singleton` inherits from. Allows singletons (or other components which exist in the scene) to have their Initialization order clearly defined in a configurable way. 

For example, you may want your UI Manager to set up before you have your Save Manager load data in and populate said UI.

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

## Utils

### `HTTPUtils`

### `MathUtils`

### `VersionUtils`