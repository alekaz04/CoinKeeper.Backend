using NpgsqlTypes;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace CoinKeeper.Logging;

public class GuidColumnWriter : ColumnWriterBase
{
    public GuidColumnWriter(NpgsqlDbType dbType, bool skipOnInsert = false, int? order = null) : base(dbType, skipOnInsert, order)
    {
    }

    public override object? GetValue(LogEvent logEvent, IFormatProvider? formatProvider = null)
    {
        return Guid.NewGuid();
    }
}
