using FluentValidation;

namespace GameXmlReader.Models;

public record Settings
{
    public bool? FlagWhenAdultFieldIsTrue { get; init; }

    public bool? FlagUnidentifiedGames { get; init; }

    public Target Target { get; init; }

    public FlaggedTerms FlaggedTerms { get; set; }
}

public class SettingsValidator : AbstractValidator<Settings>
{
    public SettingsValidator()
    {
        RuleFor(x => x.FlagWhenAdultFieldIsTrue).NotNull();
        RuleFor(x => x.FlagUnidentifiedGames).NotNull();

        RuleFor(x => x.Target)
            .NotNull()
            .SetValidator(x => new TargetValidator());

        RuleFor(x => x.FlaggedTerms)
            .NotNull()
            .SetValidator(x => new FlaggedTermsValidator());
    }
}

public record Target
{
    public string Directory { get; init; }

    public string Xml { get; init; }
}

public class TargetValidator : AbstractValidator<Target>
{
    public TargetValidator()
    {
        RuleFor(x => x.Directory)
            .NotEmpty()
            .Must(x => new DirectoryInfo(x).Exists)
            .WithMessage(x => $"Cannot find directory, {x}. Directory can be change in appsettings.json");

        RuleFor(x => x.Xml)
            .NotEmpty()
            .Must(x => new FileInfo(x).Exists)
            .WithMessage(x => $"Cannot find XML file, {x}. XML file path can be change in appsettings.json");
    }
}

public record FlaggedTerms
{
    public List<string> Names { get; init; }

    public List<string> Genres { get; init; }

    public List<string> Words { get; init; }

    public List<string> Publishers { get; init; }
}

public class FlaggedTermsValidator : AbstractValidator<FlaggedTerms>
{
    public FlaggedTermsValidator()
    {
        RuleFor(x => x.Names).NotNull();
        RuleFor(x => x.Genres).NotNull();
        RuleFor(x => x.Words).NotNull();
        RuleFor(x => x.Publishers).NotNull();

        RuleFor(x => x.Genres.Count + x.Words.Count + x.Publishers.Count)
            .GreaterThan(0)
            .WithMessage(x=> "There are no flagged terms. Add some in appsettings.json");
    }
}