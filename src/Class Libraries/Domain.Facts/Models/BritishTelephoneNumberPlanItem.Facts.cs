namespace Cavity.Models
{
    using Xunit;

    public sealed class BritishTelephoneNumberPlanItemFacts
    {
        [Fact]
        public void a_definition()
        {
            Assert.True(new TypeExpectations<BritishTelephoneNumberPlanItem>()
                            .DerivesFrom<object>()
                            .IsConcreteClass()
                            .IsUnsealed()
                            .HasDefaultConstructor()
                            .IsNotDecorated()
                            .Result);
        }

        [Fact]
        public void prop_AreaCode()
        {
            Assert.True(new PropertyExpectations<BritishTelephoneNumberPlanItem>(x => x.AreaCode)
                            .IsNotDecorated()
                            .IsAutoProperty<string>()
                            .Result);
        }

        [Fact]
        public void prop_DialingCode()
        {
            Assert.True(new PropertyExpectations<BritishTelephoneNumberPlanItem>(x => x.DialingCode)
                            .IsNotDecorated()
                            .IsAutoProperty<string>()
                            .Result);
        }

        [Fact]
        public void prop_Use()
        {
            Assert.True(new PropertyExpectations<BritishTelephoneNumberPlanItem>(x => x.Use)
                            .IsNotDecorated()
                            .IsAutoProperty<string>()
                            .Result);
        }
    }
}