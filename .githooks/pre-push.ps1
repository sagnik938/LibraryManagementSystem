#!/bin/sh
powershell.exe -ExecutionPolicy Bypass -File ".githooks/pre-push.ps1"
exit $?
