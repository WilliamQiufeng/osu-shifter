using CommandLine;
using OsuParsers.Decoders;

return Parser.Default.ParseArguments<ShiftOptions>(args)
    .MapResult(opts =>
        {
            RunShift(opts);
            return 0;
        },
        _ => 1);

int Shift(int time, ShiftOptions opts)
{
    if (time < opts.StartTime || time > opts.EndTime)
        return time;
    return time + opts.Offset;
}

void RunShift(ShiftOptions opts)
{
    var path = opts.InputPath;
    if (!File.Exists(path))
    {
        Console.Error.WriteLine($"Input file not found: {path}");
        return;
    }

    var beatmap = BeatmapDecoder.Decode(path);

    foreach (var hitObject in beatmap.HitObjects)
    {
        hitObject.StartTime = Shift(hitObject.StartTime, opts);
        if (hitObject.EndTime != 0)
            hitObject.EndTime = Shift(hitObject.EndTime, opts);
    }

    foreach (var timingPoint in beatmap.TimingPoints)
        timingPoint.Offset = Shift(timingPoint.Offset, opts);

    beatmap.GeneralSection.PreviewTime = Shift(beatmap.GeneralSection.PreviewTime, opts);

    var outputPath = string.IsNullOrWhiteSpace(opts.Output)
        ? Path.ChangeExtension(path, ".shifted.osu")
        : opts.Output;

    beatmap.Save(outputPath);
    Console.WriteLine($"Saved shifted beatmap to {outputPath}");
}

[Verb("shift", true, HelpText = "Shift a beatmap by an offset in milliseconds.")]
// ReSharper disable once ClassNeverInstantiated.Global
internal class ShiftOptions
{
    [Value(0, MetaName = "input-path", HelpText = "Input .osu file path.", Required = true)]
    public required string InputPath { get; set; }

    [Value(1, MetaName = "offset", HelpText = "Offset in milliseconds (can be negative).", Required = true)]
    public int Offset { get; set; }

    [Value(2, MetaName = "output", HelpText = "Output file path.", Required = false)]
    public required string Output { get; set; }

    [Option("startTime", Default = int.MinValue, HelpText = "Start time to apply shifting")]
    public int StartTime { get; set; }

    [Option("endTime", Default = int.MaxValue, HelpText = "End time to apply shifting")]
    public int EndTime { get; set; }
}