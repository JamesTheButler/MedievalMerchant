# Tables

Translations are organized into tables (.csv files) which help keep the project structured. Typically, there are tables for distinct sections/features in the game.
If you end up testing the game, the name of the table should give you an idea of where to find the strings in-game.

For most tables, I provide a .md file that contains further information about how the table is organized and some common patterns for the strings within it.

# Strings

Translatable strings are defined by
 - a unique key, for example 'Campsite.Storage.Description'. This is used from code.
 - a unique number. This is used from code.
 - a description. This is optional. I sometimes add them to describe how a string is used, to give some more context or explain the parameters a bit more to aid with grammar and tone.
 - an English translation. That's the original. 

 # Parameters

Parameters are a way to make strings dynamic. They are always represented with curly braces {}. I can replace these parameters from code to insert anything I want.
Here are some examples
- "Cost: {0} coin" can be changed to "Cost: 100 coin" or "Cost: 9999 coin" from code
- "Funds: {_int_Current:0.#} ({Change})" could become "Funds: 110 (+5)" from code

As you can see, parameters can look quite different. This is mostly for technical reasons.
The most imporant thing is that you should copy parameters straight from the English original. They should look idential and anything within the parameter braces should not be translated. Characters must remain in the Latin alphabet.
For example, the English original "Funds: {Current:0.#} ({Change})"
is translated to "Vermögen: {Current:0.#} ({Change})" in German. Notice how the parameters {Current} and {Change} remain untranslated, i.e. stay English.

If the grammar of your language requires it, you can move paramaters wherever you want, as long as the contents of the braces {} stay identical to the English original. I will typically use the description of a string to describe what the parameters mean.