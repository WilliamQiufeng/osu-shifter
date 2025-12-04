using CommandLine;
using OsuParsers.Decoders;

return Parser.Default.ParseArguments<ShiftOptions>(args)
    .MapResult(opts =>
        {
            RunShift(opts);
            return 0;
        },
        _ => 1);

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
        hitObject.StartTime += opts.Offset;
        if (hitObject.EndTime != 0)
            hitObject.EndTime += opts.Offset;
    }

    foreach (var timingPoint in beatmap.TimingPoints)
        timingPoint.Offset += opts.Offset;

    beatmap.GeneralSection.PreviewTime += opts.Offset;

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
}