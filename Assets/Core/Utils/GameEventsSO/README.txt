Taken from these videos:
https://www.youtube.com/watch?v=iXNwWpG7EhM
https://www.youtube.com/watch?v=P-U7GPXMtLY

To use an existing type <T> of event/listener...
1. create the event asset in Core/Game Events/
2. the triggering script must have a reference to the event asset & call .Raise()
3. any listeners must have the relevant "Unity [type] Listener" script attached
4. The listener script must point to a function that takes in the [type]

The VoidType type can be used where no parameters are passed in

To create a new type <T> of listener e.g. AudioCue...
1. Create the 3 scripts...
	[type]GameEvent
	Unity[type]Event
	Unity[type]Listener
2. proceed as above