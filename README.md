# LogSample

## Summary

ILoggerのログファイルをNLogで実現する。

## Nuget

https://www.nuget.org/packages/NLog/4.6.8?_src=template

https://www.nuget.org/packages/NLog.Extensions.Logging/1.6.1?_src=template

## Initialize

- パスはフルパス指定
- .NetStandardプロジェクトのソースで書いてしまえばプラットフォーム毎の設定不要
- AddProviderだけでILoggerでNLogが動く

