# Use the AIS client via CLI
The AIS client can be used from the command line as a standalone tool.

## Usage
The following is a help listing with the available parameters: 
```text
--------------------------------------------------------------------------------
Swisscom AIS Client
--------------------------------------------------------------------------------
Usage: .\CLI.exe [OPTIONS]

Options:

    -input [FILE]                                       - Source PDF file to sign. You can use this parameter several times, to sign multiple
                                                          documents at once. Using multiple inputs forces the usage of suffix (see below) as it is
                                                          difficult to give both the input and the output files as parameters.

    -output [FILE]                                      - Output PDF file, where the signed document should be written. This parameter can be
                                                          used only when one single input is given. For more than one input, use suffix.

    -suffix [SUFFIX]                                    - Suffix for output file(s), composed using the input file plus the suffix, as alternative to
                                                          specifying the output file entirely. Default is "signed-#time", where "#time" is replaced
                                                          with current time (hhMMss).

    -type [static|ondemand|ondemand-stepup|timestamp]   - The type of signature to create

    -config [PROPERTIES FILE]                           - The properties file that provides the extra configuration parameters. A sample configuration is provided in config.json

    -help                                               - This help text

    -v                                                  - Be verbose. Sets logging to DEBUG. By default logging is set to INFO 


Sample use cases:
    1. Edit the config.json file accordingly
    2. > .\CLI.exe -type timestamp -config config.json -input fileIn.pdf -output fileOut.pdf
    3. > .\CLI.exe -type timestamp -config config.json -input file1.pdf -input file2.pdf -input file3.pdf -v
```

Use the _-v parameter to have more detailed logs in the console

## Examples
Start by editing the config.json file or any other similar file you intend to use as configuration. The examples will use config.json

Timestamp a PDF document:
```cmd
.\Cli.exe -type timestamp -input local-sample-doc.pdf -output timestamp-output.pdf -config config.json
```

Timestamp several PDF documents at once:
```cmd
.\Cli.exe -v -type timestamp -input doc1.pdf -input doc2.pdf -input doc3.pdf -config config.json

Sign a PDF document with a static signature:
```cmd
.\Cli.exe -v -type static -input doc1.pdf -config config.json
```

Sign a PDF document with a static signature with extra logging info and specifying the output and config files:
```cmd
.\Cli.exe -v -type static -input doc1.pdf -output doc1Out.pdf -config config.json
```

Sign a PDF document with an On Demand signature:
```cmd
.\Cli.exe -type ondemand -input doc1.pdf -output doc1Out.pdf -config config.json
```

Sign a PDF document with an On Demand signature with Step Up and complete extra logging:
```cmd
.\Cli.exe -type ondemand-stepup -input local-sample-doc.pdf -output signature-output.pdf -config config.json
```

Notes:

- if you use more input files, then the _output_ parameter cannot be used. Instead, you can rely on the _suffix_ parameter to define a 
  suffix to be added to the name of the input files when generating the output ones. For the _suffix_, you can also use a token like 
  _#time_ to have it replaced with the current date and time. By default, the _suffix_ is "-signed-#time".
  
