# MyTestLib ![.NET Core](https://github.com/s4rduk4r/MyTestLib/workflows/.NET%20Core/badge.svg?branch=master&event=push)
**MyTestLib** is a MyTestEngine done as a library

## Test file structure
Every meaningful line has syntax: __Key = Value__

Spaces between the fields can be unlimited.

Commentary lines start from __#__ symbol.

**Rules to follow:**
1. Keywords __Test__, __Author__, __Date__, __Time__, __Mode__, __Question__ can be used in any order and in any place of the text
2. Keywords __Value__, __Answer__, __Answer+__ are ignored if placed before keyword __Question__
3. Keywords __Value__, __Answer__, __Answer+__ are considered to be related to the latest occurence of the __Question__ keyword

**Supported keywords:**
1. __Test__ - test name
2. __Author__ - Test author
3. __Date__ - Creation/Modification date. Must be in format *DD.MM.YYYY*
4. __Time__ - Test time in seconds. Note that this is a time for the whole test.
5. __Mode__ - Test mode. Can be either *Loyal* or *Punish*.
*Punish* mode means wrong answers give negative points worth question value divided on all wrong answers.
*Loyal* is the default mode and doesn't account negative points.
6. __Question__ - Each question begins with keyword **Question**
7. __Value__ - how much points is given question worth
8. __Answer__ - Every keyword **Answer** is counted towards the latest encountered **Question** keyword
9. __Answer+__ - Correct answers are marked by *+* suffix


## Example test file
Example test file can be found here -> https://github.com/s4rduk4r/MyTest/blob/master/MyTest/Tests/TestExample.mytest
