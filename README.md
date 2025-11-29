# osu!shifter

This is just a simple utility that can shift the timing of
all hit objects, timing points and preview point of an
osu! beatmap by a specified amount of milliseconds.

此工具可以将osu!谱面的所有物件、时间点和预览点的时间整体前后移动指定的毫秒数。

## Usage 使用方法

```csharp
osu_shifter <input file> <shift amount in ms> <output file>
```

For example, to shift all timing by -500ms (i.e., 0.5 seconds earlier):

比如，要将所有时间点提前500毫秒，可以使用如下命令：

```csharp
osu_shifter "C:\path\to\input.osu" -500 "C:\path\to\output.osu"
```