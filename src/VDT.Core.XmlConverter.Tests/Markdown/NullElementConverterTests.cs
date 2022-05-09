﻿using System.IO;
using VDT.Core.XmlConverter.Markdown;
using Xunit;

namespace VDT.Core.XmlConverter.Tests.Markdown {
    public class NullElementConverterTests {
        [Fact]
        public void RenderStart() {
            using var writer = new StringWriter();

            var converter = new NullElementConverter("foo", "bar");

            converter.RenderStart(ElementDataHelper.Create("bar"), writer);

            Assert.Equal("", writer.ToString());
        }

        [Fact]
        public void RenderEnd() {
            using var writer = new StringWriter();

            var converter = new NullElementConverter("foo", "bar");

            converter.RenderEnd(ElementDataHelper.Create("bar"), writer);

            Assert.Equal("", writer.ToString());
        }
    }
}
