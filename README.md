# Grammophone.LanguageModel.Greek.Provision
This library provides a `LanguageProvider` implementation for the ancient Greek language according to the contract set in library [Grammophone.LanguageModel](https://github.com/grammophone/Grammophone.LanguageModel), as shown in the UML diagram:

![Greek language provider](http://s22.postimg.org/x3q7iyeap/Greek_language_provider.png)

It also defines two additional Greek-specific syllable distance metrics yielded by `GreekSyllabizer`, the `ReplaceConsonantsCommand` command and the `ReplaceVowelsCommand` shown below.

![Greek-specific edit commands](http://s10.postimg.org/8fnrnty7t/Specific_Greek_edit_commands.png)

This project depends on the following projects residing in sibling directories:
* [Grammophone.GenericContentModel](https://github.com/grammophone/Grammophone.GenericContentModel)
* [Grammophone.LanguageModel](https://github.com/grammophone/Grammophone.LanguageModel)
