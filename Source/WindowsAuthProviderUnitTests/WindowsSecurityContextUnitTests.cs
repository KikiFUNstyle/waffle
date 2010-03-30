using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.ComponentModel;
using NUnit.Framework;

namespace Waffle.Windows.AuthProvider.UnitTests
{
    [TestFixture]
    public class WindowsSecurityContextUnitTests
    {
        [Test]
        public void TestNegotiate()
        {
            string package = "Negotiate";
            using (WindowsCredentialsHandle credentialsHandle = new WindowsCredentialsHandle(
                string.Empty, Secur32.SECPKG_CRED_OUTBOUND, package))
            {
                using (WindowsSecurityContext context = new WindowsSecurityContext(
                    WindowsIdentity.GetCurrent().Name,
                    credentialsHandle,
                    package))
                {
                    Assert.AreNotEqual(context.Context, Secur32.SecHandle.Zero);
                    Assert.IsNotNull(context.Token);
                    Assert.IsNotEmpty(context.Token);
                    Console.WriteLine(Convert.ToBase64String(context.Token));
                }
            }
        }

        public void TestGetCurrentNegotiate()
        {
            using (WindowsSecurityContext context = WindowsSecurityContext.GetCurrent("Negotiate"))
            {
                Assert.AreNotEqual(context.Context, Secur32.SecHandle.Zero);
                Assert.IsNotNull(context.Token);
                Assert.IsNotEmpty(context.Token);
                Console.WriteLine(Convert.ToBase64String(context.Token));
            }
        }

        [Test]
        public void TestGetCurrentNTLM()
        {
            using (WindowsSecurityContext context = WindowsSecurityContext.GetCurrent("NTLM"))
            {
                Assert.AreNotEqual(context.Context, Secur32.SecHandle.Zero);
                Assert.IsNotNull(context.Token);
                Assert.IsNotEmpty(context.Token);
                Console.WriteLine(Convert.ToBase64String(context.Token));
            }
        }

        [Test, ExpectedException(typeof(Win32Exception), ExpectedMessage = "The requested security package does not exist")]
        public void TestGetCurrentInvalidPackage()
        {
            using (WindowsSecurityContext context = WindowsSecurityContext.GetCurrent(Guid.NewGuid().ToString()))
            {
                Assert.AreNotEqual(context.Context, Secur32.SecHandle.Zero);
                Assert.IsNotNull(context.Token);
                Assert.IsNotEmpty(context.Token);
                Console.WriteLine(Convert.ToBase64String(context.Token));
            }
        }
    }
}