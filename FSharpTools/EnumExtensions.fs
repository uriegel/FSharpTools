module EnumExtensions

open System

let hasFlag (enum: Enum) flag = enum.HasFlag flag
