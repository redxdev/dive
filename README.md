Dive Engine
===========

Dive Engine is a component-based 2d game engine written in C#. It uses an entity-component model (not
an entity-component-system model as is sometimes used) to handle game elements.

Alpha notice
------------
Dive Engine is currently in alpha! That means that it is not production ready and it is not
feature-complete! Don't use it expecting everything to work as planned.

Dependency notice
-----------------
Dive requires the nuget package "antlr4".

Tutorials
=========

You can find the official tutorial series at http://redxdev.com/category/code/tutorials/dive-engine/

Licence
=======

Dive Engine is under the MIT licence. You can find the full terms of the licence in the LICENCE file.

Building
========

Dive Engine should build out of the box with Visual Studio 2012, as all dependencies are included.
Building for systems other than windows will require rebuilding the dependencies and building both
SFML and CSFML.

Debugging
=========

Debugging under Visual Studio 2012 will fail initially (or rather it will pull up a blank console window
which will close after a few seconds) unless you set the working directory of the "Engine" project to
"Dist".

Running
=======

The engine must be run with the current working directory set as the Dist folder. Other than that, it should
just run.

Differences between debug and release builds
============================================

There is a limited amount of conditional compilation based on debug and release constants, to change behaviour
during debugging.

The most obvious is exception handling. If an exception is thrown and not caught until the main method, the engine
can behave in two different ways. In debug mode, the exception is left alone, so that the Visual Studio debugger
can detect it. In release mode, the exception is caught, logged, and the program exits.

Logging
=======

Make sure to clean out the logs directory every so often; the logs can get large very fast as the default
logging settings log all messages. To change this, edit config/logging.xml and change the logging level
from DEBUG.