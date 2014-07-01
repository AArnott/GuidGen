namespace GuidGenTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using GuidGen;
    using Xunit;

    public class GuidGenViewModelTests
    {
        private GuidGenViewModel viewModel = new GuidGenViewModel();

        [Fact]
        public void InitialGuidIsNonEmpty()
        {
            Assert.NotEqual(Guid.Empty, this.viewModel.Value);
        }

        [Fact]
        public void NewGuidChangesGuid()
        {
            Guid originalGuid = this.viewModel.Value;
            this.viewModel.NewGuid();
            Assert.NotEqual(originalGuid, this.viewModel.Value);
            Assert.NotEqual(Guid.Empty, this.viewModel.Value);
        }

        [Fact]
        public void NewGuidCommandCanExecute()
        {
            Guid originalGuid = this.viewModel.Value;
            Assert.True(this.viewModel.NewGuidCommand.CanExecute(null));
        }

        [Fact]
        public void NewGuidCommandChangesGuid()
        {
            Guid originalGuid = this.viewModel.Value;
            this.viewModel.NewGuidCommand.Execute(null);
            Assert.NotEqual(originalGuid, this.viewModel.Value);
            Assert.NotEqual(Guid.Empty, this.viewModel.Value);
        }
    }
}
