# DickinsonBros.Logger

A logging service that redacts all logs

Features
* Redacts all logs
* Allows for dictionary of variables to be past in that all become first class propertys in the log. (Easyer to query logs, and view them)
* Ability to add a correlation id that works though async in straight forward fashion
* Allows for improved testability
* Sperate abstractions library to reduce coupling of packages.
