// Copyright 2022 Carl Reinke
//
// This file is part of a program that is licensed under the terms of the GNU
// General Public License Version 3 as published by the Free Software
// Foundation.

using System.Diagnostics;
using System.Text;
using Xunit;

namespace SKSshAgent.Ssh.Tests
{
    public static class OpenSshEncryptedPrivateKeyTests
    {
        private static readonly string _openSshGeneratedUnencryptedKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAAEbm9uZQAAAAAAAAABAAAAfwAAACJzay1lY2
RzYS1zaGEyLW5pc3RwMjU2QG9wZW5zc2guY29tAAAACG5pc3RwMjU2AAAAQQQYseJGSXsU
/OpWury3byC2mn2f1LeaFD/u6XH7xRgOAU9fbFa+budMYSpvUOZRh/VIyfAYvFqbgoMAJv
sQbAS/AAAABHNzaDoAAADYyqVrCcqlawkAAAAic2stZWNkc2Etc2hhMi1uaXN0cDI1NkBv
cGVuc3NoLmNvbQAAAAhuaXN0cDI1NgAAAEEEGLHiRkl7FPzqVrq8t28gtpp9n9S3mhQ/7u
lx+8UYDgFPX2xWvm7nTGEqb1DmUYf1SMnwGLxam4KDACb7EGwEvwAAAARzc2g6AQAAADDX
fpqIuj84e2ePdKANw8/HLgMXBUdcR33lourkTG/9Tosj4aVDDvhYiD9ZdhuzqGQAAAAAAA
AAEGNhcmxAZXhhbXBsZS5jb20BAgME
-----END OPENSSH PRIVATE KEY-----
";

        private static readonly (SshKey Key, string Comment) _openSshGeneratedUnencryptedKeyAndComment = SshKey.ParseOpenSshPrivateKey(_openSshGeneratedUnencryptedKeyFile);

        public static readonly TheoryData<TryDecrypt_OpenSshGeneratedEncrypted_TestCase> TryDecrypt_OpenSshGeneratedEncrypted_Data = new()
        {
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAACmFlczEyOC1jYmMAAAAGYmNyeXB0AAAAGAAAABASkDrpJI
XGGPbMBdwUokD6AAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3Bl
bnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcf
vFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOgAAAOCyr1s4
WmOyiB7lEPna3nJpAfIHGiJgGv8bhvBqir/DM6Hat+0XC7RpDPxct5+jbklC0uYzOR4Vsg
0djX0aiyz0p4qdx8NGelM2rFPHbbGVIXoRRgE2V0M7SvyT/tBAD95qJGtYNg+xlrbQTrYa
Gq7/7CIGLkMGpmhVdDoVdeaUoqmwRPwJARFRX80zcVB8mOPrQo6WMCDEzsv81rIhHM3gda
YebUhgk5u9YJVbXWAWjPDrEt52EqRU0spqh9iUFl9az5RES3xyfWc8I294zYhfeLdaX2vf
ue2JAgrfBnWygg==
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.Aes128Cbc,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAACmFlczE5Mi1jYmMAAAAGYmNyeXB0AAAAGAAAABDt5QQPys
rv9lV/7btSNtwnAAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3Bl
bnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcf
vFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOgAAAOBUTEMA
UIFu9lpaVFKiUU+ZkWYmR37s+a/Uknx4SI78adhrVcnrkA63CYJzfsK6V85bEZpC2vOzhq
MtF8LDIDUIHgUSrbA2nik2ZO6/8P9QpuU5xy3SJbcIAXkNxCvL8y5/6dRfkSPA00//BwZD
uP1/Np07JiGHucITJ1IIwEAahLre/BqZbbYiDVkOcFjfWdUX/sXBCjPNeK1b/odpa1jwn8
8L7nFWcv45KE1Jxk7L1MN32j7imAcSGvWaGgkoe7ukRodNhi1dJGR73NI2ys9zlU7aycKJ
4S+8vnC9nqQhYQ==
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.Aes192Cbc,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAACmFlczI1Ni1jYmMAAAAGYmNyeXB0AAAAGAAAABBlRY3hMw
QZB23+zHgnI0OxAAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3Bl
bnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcf
vFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOgAAAODAn16Q
gV2i53kk8bjmtW6a3UvdY73cnylDMqn/A9Wf9WsEtE/kCBOamSQxTXtWfjvvCFB0QIX6k6
Xy5RWk2IC/CilBSk0brWPtBid5lrQGbuhIqpxt/QIdQkkVVcRZVvAlZ1mXnEeTyrqRruTz
j3Fg+qvQp0Oh5Mbw4vXAgDZ3gzvGieP5dtzu31D4A97jOVg65lNiNMEKuKHgihDKkJoK4n
k+ofSngBY8qokMlWdAjn+3y11EYzuygqb+sfTBflolM59LDeYdDJoG+k/emUeHkWl10/GS
uRZXrxUQ5NfUEg==
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.Aes256Cbc,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAACmFlczEyOC1jdHIAAAAGYmNyeXB0AAAAGAAAABDXgjv3ll
u3n2KzysVNeVPzAAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3Bl
bnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcf
vFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOgAAAOAzX6X2
O0cytzvSd7qug388EDi1kJHqhsSkBDvOKHjSSXreyXkypphD/M7NP38avpDRuZ5Sv/e4y1
wvHKVJ5wF0FXWYs+zwYdmvfsPmABbarki7l1woYV50roiY7fUdmYiYElvxvrWH7hZoyl/C
t/8qZOyN4XVvvTyTdAcnfkqMq9nlQUFsTfTXffRNx3lUx2qsok41Tp1vX9j0GVZzH+EbCU
HbsQFr7bGTn6Mikbh2DLVbUpJWWAn5LYDZss9txIPl7KYw88/v34SsTUWyBdc9kPBSf1pZ
CzIPD1G7dQEf/g==
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.Aes128Ctr,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAACmFlczE5Mi1jdHIAAAAGYmNyeXB0AAAAGAAAABBC6qvSo+
4/EWSoybZAXqjFAAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3Bl
bnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcf
vFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOgAAAODzS0Y7
yqk1HBwje3wMTxa0Z6A4TSftzkjudpVvJFgJ1cNKEz3vAdW8t/kITk3bGev5X3bgJCvVXL
YJLCqGe4YOPd5V+f1l7AtGO4iTS1nduKAX+w8xBqhr7z/dFIhg96dwvEAmDeFPbrxLwp+Z
blMoc7WpYUpXQMCQTYGpizbCDa2tciZYlx0TVfNOfYigjGLcAeSrGEjRPGWESDkHCnFkQs
q+13CK+oKm+Kr/ru1th/KPypFmu4aWNQVqLXUjszvMOuUrJS+xZGZTtyrwkjKuuBgOQIIf
boWk48+4oPHwIg==
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.Aes192Ctr,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAACmFlczI1Ni1jdHIAAAAGYmNyeXB0AAAAGAAAABCumssFg/
dfcniu0wqz1531AAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTItbmlzdHAyNTZAb3Bl
bnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvILaafZ/Ut5oUP+7pcf
vFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAEc3NoOgAAAODQ46tB
D7E+BxnoQd4WBj6RwRneldeFVrpCgWnqNlKaSLzl6Xtft9xeELCrWUhfG5Sreda2qao0Ni
Wgvy1naCwSWuEdqQ2O0Uy7h2YUSufYdGXWU8Elb8gUGOABb9wvb/IstTua1Z7hn6XQtNaY
CQtmmReohcrfSCwiThr8H2qN4JHbUa7zcOh0/5ZF/sI4JDKE8yT/9LFqguUk/m/0/qJwrN
rO698MajHAEI98kCrtTbfyQXMafKk5LLb1CXvY2Mchkkl8lL1n41AyfsmufX9RSs7mwxxf
eiJTYsQCg3VMXA==
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.Aes256Ctr,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAAFmFlczEyOC1nY21Ab3BlbnNzaC5jb20AAAAGYmNyeXB0AA
AAGAAAABDBy2ghmKgb9g0ipLyx/TXZAAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTIt
bmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvIL
aafZ/Ut5oUP+7pcfvFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAE
c3NoOgAAAOAJ0GsIZbM6Jot4cQeziL13QonG/F686w9AZXWIKZT41m1LX6Vi2tMTElSfHA
CokrlKgiNNiV6Zb4q7ecTNxvrzF4jIOGVl4iXNcwmUxy61rnU/D9oJes05/fKK5LqlsftN
mhG8o0E2fIthQS1G+GDqv+yITNN6r7+XtCE2Cjn6ZlnUQyoKyJy6ZMVms1wolVujdmCE+B
F9lf/Llq19NoKncZX23sSBVvL4Wm1Jomstqiu52MfIMwLd5VgdgZ2ZmPSDZLF05J+JgdKl
PeiAeuyAqJwTGl1DSwzuUiYP9O3Rz/m5OB3VbY5bfFlOUpK3Ik8=
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.OpenSshAes128Gcm,
            },
            new()
            {
                PrivateKeyFile =
@"-----BEGIN OPENSSH PRIVATE KEY-----
b3BlbnNzaC1rZXktdjEAAAAAFmFlczI1Ni1nY21Ab3BlbnNzaC5jb20AAAAGYmNyeXB0AA
AAGAAAABDfaT/FrZWaA9pfNiDggx0xAAAAEAAAAAEAAAB/AAAAInNrLWVjZHNhLXNoYTIt
bmlzdHAyNTZAb3BlbnNzaC5jb20AAAAIbmlzdHAyNTYAAABBBBix4kZJexT86la6vLdvIL
aafZ/Ut5oUP+7pcfvFGA4BT19sVr5u50xhKm9Q5lGH9UjJ8Bi8WpuCgwAm+xBsBL8AAAAE
c3NoOgAAAODREdys8z7JLMsFP2VuKVYKvxaP8USndBtKYlOVsF/kZk5y6PeIq3+158O4Yi
1FuVXiR0NKUPs1LSuR+VCbJH7c05wDGIg8wmkgsPDXXkXGqdPJU701NJt3BvrSyZqdCaPJ
9M0Of/f1MkwHNq7Jc+6e3gbRftprTi23sPUk/Nc+xgH1eR1fbyYjJdYYCEy91UYAL7vAmo
5aQ+o4FNXOtlVdXWN0+sH+ZxuR3VR1g+ZxqswkBP+pBKtUpEqYKNFubkh8RKSw91rYZc6O
qtBTHGBj9rPm365ej86j0bNdxxnpfRrX1O20D6+LrZ9KwAwcntE=
-----END OPENSSH PRIVATE KEY-----
",
                CipherInfo = SshCipherInfo.OpenSshAes256Gcm,
            },
        };

        [Theory]
        [MemberData(nameof(TryDecrypt_OpenSshGeneratedEncrypted_Data))]
        public static void TryDecrypt_OpenSshGeneratedEncrypted_Succeeds(TryDecrypt_OpenSshGeneratedEncrypted_TestCase testCase)
        {
            (var privateKey, string comment) = SshKey.ParseOpenSshPrivateKey(testCase.PrivateKeyFile);

            var encryptedPrivateKey = (OpenSshEncryptedPrivateKey)privateKey.EncryptedPrivateKey!;

            Assert.Equal(testCase.CipherInfo, encryptedPrivateKey.CipherInfo);

            var password = ShieldedImmutableBuffer.Create(Encoding.UTF8.GetBytes("test"));

            bool result = encryptedPrivateKey.TryDecrypt(password, out var decryptedPrivateKey, out string? decryptedComment);

            Assert.True(result);
            Assert.Equal(_openSshGeneratedUnencryptedKeyAndComment.Key, decryptedPrivateKey);
            Assert.Equal(_openSshGeneratedUnencryptedKeyAndComment.Comment, decryptedComment);
        }

        public struct TryDecrypt_OpenSshGeneratedEncrypted_TestCase
        {
            public string PrivateKeyFile;
            internal SshCipherInfo CipherInfo;
        }

        public static readonly TheoryData<string, uint, string> Encrypt_Always_Data = new()
        {
            { SshKdfInfo.None.Name, 0, SshCipherInfo.None.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.Aes128Cbc.Name },
            { SshKdfInfo.Bcrypt.Name, 32, SshCipherInfo.Aes128Cbc.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.Aes192Cbc.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.Aes256Cbc.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.Aes128Ctr.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.Aes192Ctr.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.Aes256Ctr.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.OpenSshAes128Gcm.Name },
            { SshKdfInfo.Bcrypt.Name, 16, SshCipherInfo.OpenSshAes256Gcm.Name },
        };

        [Theory]
        [MemberData(nameof(Encrypt_Always_Data))]
        public static void Encrypt_Always_ResultCanBeDecrypted(string kdfName, uint kdfRounds, string cipherName)
        {
            if (!SshKdfInfo.TryGetKdfInfoByName(kdfName, out var kdfInfo))
                throw new UnreachableException();
            if (!SshCipherInfo.TryGetCipherInfoByName(cipherName, out var cipherInfo))
                throw new UnreachableException();

            var key = _openSshGeneratedUnencryptedKeyAndComment.Key;
            string comment = _openSshGeneratedUnencryptedKeyAndComment.Comment;

            var password = ShieldedImmutableBuffer.Create(Encoding.UTF8.GetBytes("test"));

            var encryptedPrivateKey = OpenSshEncryptedPrivateKey.Encrypt(key, comment, password, kdfInfo, kdfRounds, cipherInfo);

            Assert.Equal(kdfInfo, encryptedPrivateKey.KdfInfo);
            Assert.Equal(cipherInfo, encryptedPrivateKey.CipherInfo);

            bool result = encryptedPrivateKey.TryDecrypt(password, out var decryptedKey, out string? decryptedComment);

            Assert.True(result);
            Assert.Equal(key, decryptedKey);
            Assert.Equal(comment, decryptedComment);
        }
    }
}
