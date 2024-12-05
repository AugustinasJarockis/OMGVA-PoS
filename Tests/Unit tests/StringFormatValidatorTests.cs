using OMGVA_PoS.Helper_modules.Utilities;

namespace Utility_tests
{
    public class StringFormatValidatorTests
    {
        public static IEnumerable<object[]> Emails() {
            yield return new object[] { "alabas", false };
            yield return new object[] { "banas@bananas.lt", true };
            yield return new object[] { "alenas@nalenas", false };
            yield return new object[] { "alenas.nalenas", false };
            yield return new object[] { "breikas.alenas@nalenas.com", true };
        }

        [Theory, MemberData(nameof(Emails))]
        public void TestEmailValidity(string email, bool expected) {
            bool result = email.IsValidEmail();
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Phones() {
            yield return new object[] { "alabas", false };
            yield return new object[] { "379519565", true };
            yield return new object[] { "165s19832", false };
            yield return new object[] { "++6542615", false };
            yield return new object[] { "+386 84652 465", true };
        }

        [Theory, MemberData(nameof(Phones))]
        public void TestPhoneValidity(string phone, bool expected) {
            bool result = phone.IsValidPhone();
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Names() {
            yield return new object[] { "alabas123", false };
            yield return new object[] { "Alnestijus", true };
            yield return new object[] { "165s19832", false };
            yield return new object[] { "Kanceropas++", false };
            yield return new object[] { "Inteligentijus", true };
        }

        [Theory, MemberData(nameof(Names))]
        public void TestNameValidity(string name, bool expected) {
            bool result = name.IsValidName();
            Assert.Equal(expected, result);
        }

        public static IEnumerable<object[]> Usernames() {
            yield return new object[] { "j", false };
            yield return new object[] { "Alnestijus_123_321", true };
            yield return new object[] { "Ultralongusernamefromtheperipherytotheend", false };
            yield return new object[] { "Kanceropas++", false };
            yield return new object[] { "x_X_x_Inteligentijus_x_X_x", true };
        }

        // Uncomment when validation is fixed

        //[Theory, MemberData(nameof(Usernames))]
        //public void TestUsernameValidity(string username, bool expected) {
        //    bool result = username.IsValidUsername();
        //    Assert.Equal(expected, result);
        //}

        //public static IEnumerable<object[]> Passwords() {
        //    yield return new object[] { "slaptas", false };
        //    yield return new object[] { "Mikrobange12356", true };
        //    yield return new object[] { "LongEnough", false };
        //    yield return new object[] { "16465648", true };
        //    yield return new object[] { "Tambarambaranma", true };
        //}

        //[Theory, MemberData(nameof(Passwords))]
        //public void TestPasswordValidity(string username, bool expected) {
        //    bool result = username.IsValidPassword();
        //    Assert.Equal(expected, result);
        //}
    }
}