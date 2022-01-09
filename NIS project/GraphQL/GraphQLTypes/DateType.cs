using GraphQL.Language.AST;
using GraphQL.Types;

namespace NIS_project.GraphQL.GraphQLTypes
{
    public class DateType : ScalarGraphType
    {
        public DateType()
        {
            Name = "CustomDate";
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is NullValue)
                return null;

            if (value is StringValue stringValue)
                return ParseValue(stringValue.Value);

            return ThrowLiteralConversionError(value);
        }

        public override object ParseValue(object value)
        {
            if (value == null)
                return null;

            if (value is string dateText)
            {
                DateTime date;
                if (DateTime.TryParseExact(dateText,
                       "dd-MM-yyyy",
                       System.Globalization.CultureInfo.InvariantCulture,
                       System.Globalization.DateTimeStyles.None,
                       out date))
                {
                    return date;
                }
            }

            return ThrowValueConversionError(value);
        }

        public override object Serialize(object value)
        {
            if (value == null)
                return null;

            if (value is DateTime date)
            {
                return date;
            }

            return ThrowSerializationError(value);
        }
    }
}
