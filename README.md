# SLC-AS-ChatOps-Elements

This repository contains an automation script solution with scripts that can be used to retrieve Element information from your DataMiner system using the DataMiner Teams bot.

The following scrips are currently available:

- [Show Elements by Protocol](#Show-Elements-by-Protocol)

- [Show Elements With Parameter Value](#Show-Elements-With-Parameter-Value)

## Pre-requisites

Kindly ensure that your DataMiner system and your Microsoft Teams adhere to the pre-requisites described in [DM Docs](https://docs.dataminer.services/user-guide/Cloud_Platform/TeamsBot/Microsoft_Teams_Chat_Integration.html#server-side-prerequisites).

## Show Elements by Protocol

Automation script that returns the elements that use the given protocol from the connected DataMiner system. In case the protocol is not found or a typo is made it will suggest 3 protocols based on the input.

![Show element by Protocol example](/Documentation/ShowElementsByProtocol.gif)

## Show Elements With Parameter Value

Automation script that returns the elements that use the given protocol containing a parameter with a given value from the connected DataMiner system. This parameter can be defined by id or description. In case the protocol is not found or no matching elements are detected it will report this.

**Limitations:**

    Column parameters are not supported.

![Show Element with Parameter Value example](/Documentation/ShowElementwithParameterValue.gif)