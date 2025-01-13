using System.Reflection;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.PDF.Fonts;

public class ExpensesReportFontResolver : IFontResolver
{
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName) ?? ReadFontFile(FontHelper.DEFAULT_FONT);

        var length = stream!.Length;

        var data = new byte[length];

        stream!.Read(data, 0, (int)length);

        return data;
    }

    private Stream? ReadFontFile(string faceName)
    {
        // essa função eu consigo os arquivos dll do projeto do build
        var assembly = Assembly.GetExecutingAssembly();

        // ai como falamos que esse arquivo é um build resource antes eu consigo pegar as fontes
        return assembly.GetManifestResourceStream($"CashFlow.Application.UseCases.Expenses.Reports.PDF.Fonts.{faceName}.ttf");
    }
}