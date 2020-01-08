using System;

namespace MrgInfo.AdoQuery.Core.FormattableStrings
{
    sealed class FormattableStringWithId: FormattableString
    {
        public string Id { get; }

        FormattableString FormattableString { get; }

        public FormattableStringWithId(string id, FormattableString formattableString)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            FormattableString = formattableString ?? throw new ArgumentNullException(nameof(formattableString));
        }

        public override object[] GetArguments() => FormattableString.GetArguments();

        public override object GetArgument(int index) => FormattableString.GetArgument(index);

        public override string ToString(IFormatProvider formatProvider) => FormattableString.ToString(formatProvider);

        public override string Format => FormattableString.Format;

        public override int ArgumentCount => FormattableString.ArgumentCount;
    }
}
