SRCS = $(shell find . -name '*.cs' -and -not -name '*test*' -and -not -path './Demo/*')

build:
	mcs /out:Example.exe $(SRCS)

clean:
	rm Example.exe

.PHONY: clean
