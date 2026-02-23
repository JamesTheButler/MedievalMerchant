Here is the plan.

I want to source generate code that extracts informations from two types of unity assets : String Tables and Shared Table Datas. For this, there is a source generation solution which has some things stubbed out in C:\Projects\MedievalMerchant\SourceGenerators\Unity.Localization.Roslyn.

the csc.rsp file at this location: "C:\Projects\MedievalMerchant\Assets\csc.rsp" contains all relevant files. The *SharedData.asset files contain serialized SharedTableDatas with keys of translatable strings and their unique ID.
The *_en.asset files contain StringTables that contain all other requried data.
Both files together contain all data to do what i need to do.

We can assume that the name of the table can be found in both files like this <TableName>SharedData.asset and <TableName>_en.asset. Both assets also contain ids that map them to one another. You can choose how to handle this mapping.

In the end, I want to be able to retrieve any key from any table like in this example:
Table: SourceGenTest
Key: SourceGenTest.Normal.Example
Generated Method: LOC.SourceGenTest.NormalExample(); // this returns the translated string

Here is an example with a bunch of parameters.
Table: SourceGenTest
Key: SourceGenTest.SmartString.Multiple.Mixed
Value: int: {_int_Arg1}, string: {_string_Arg2}, default: {Arg3}, default2:{Arg4}
Generated Method: LOC.SourceGenTest.SmartStringMultipleMixed(int Arg1, string Arg2, string Arg3, string Arg4); // this returns the translated string with all placeholders put in.