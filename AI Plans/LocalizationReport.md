# The Goal
I need a script that generates a change report that lists changes in localization files which are relevant to translators.
I will supply the commit id. The script checks all git commits since the provided commit and the current commit to output a table with two sections:
1. a new localized string has been added
2. the english text for an existing localized string has changed

To clarify, it does not matter for the French translations if a German translation has changed. Only the original English is relevant.


# Unity Localization System
- unitys localization system uses .asset files in the yaml format
- strings are organized into so called string tables
- There is one file per table per language.
- I use English as the base langauge based on which everything should be translated.
- There is one file per table that defines some meta-data as well as the keys for each string.
- The naming scheme for these files is as follows:
  - The file containing meta data and keys is called '<TableName>SharedData.asset'
  - Each language file containing the translations is called '<TableName>_<LanguageKey>.asset'
  - Language keys: German: 'de', French: 'fr'
  - Example: for a table named 'Common' the files will look like this:
    - meta file: 'CommonSharedData.asset'
    - Egnlish file 'Common_en.asset'
- entries in the english language file looks like this. m_Id is the fixed id for each localized string entry
"  - m_Id: 1907952148480
    m_Localized: 'The Localized String Value'"
- entries in the shared table look like this
"    - m_Id: 13422780416
      m_Key: Common.Placeholder"
- entries in both types of table have other properties, that can safely be ignored.


# My Setup
- I will run the script from the ./Scripts folder.
- The script shall be named 'LocReport'.
- All tables are in the same folder in my project: ./Assets/Features/Localization/Data/Tables/
- I have git, python, windows cmd line, powershell installed. I am OK with downloading other tools if needed.


# Edge Cases
- If a new string has been added, then deleted, it does not neet to appear in the change report.
- If a new string has been added, then renamed (i.e. the key has been renamed), it should appear in the report with the latest key.
- In general, only the latest key should be used in the report, even after multiple renames.


# Error Handling
- commit sha that could not be found
- commit sha that is too far back. i don't want to accidentally handle 500 comits. Only implement this if it's reasonably easy.
- invalid language key


# Thinks to sort out
1. Which language is best suited to utilize git tools and generate the report?
2. Which output format is best suited for the report? What type of file or data should be generated?
3. Are there any other uncertainties or things that need clarification?
4. What other edge cases can you think off?
5. We should consider handling the edge cases in a separate step. It feels like they would bloat the script a lot.
6. Do we need more error handling?