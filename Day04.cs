using System.Diagnostics;
using System.IO;
using System.Linq;
using Utilities;

class Day04 : IDay
{
    struct Passport
    {
        public Dictionary<string, string> Fields;

        public bool IsValid()
        {
            if (!Fields.ContainsKey("byr"))
            {
                return false;
            }
            // byr (Birth Year) - four digits; at least 1920 and at most 2002
            int birthYear = int.Parse(Fields["byr"]);
            if (!birthYear.InRange(1920, 2002))
            {
                return false;
            }

            if (!Fields.ContainsKey("iyr"))
            {
                return false;
            }
            // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
            int issueYear = int.Parse(Fields["iyr"]);
            if (!issueYear.InRange(2010, 2020))
            {
                return false;
            }

            if (!Fields.ContainsKey("eyr"))
            {
                return false;
            }
            // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
            int expirationYear = int.Parse(Fields["eyr"]);
            if (!expirationYear.InRange(2020, 2030))
            {
                return false;
            }

            if (!Fields.ContainsKey("hgt"))
            {
                return false;
            }
            // hgt (Height) - a number followed by either cm or in:
            //    If cm, the number must be at least 150 and at most 193.
            //    If in, the number must be at least 59 and at most 76
            string height = Fields["hgt"];
            if (height.EndsWith("cm"))
            {
                int heightInCm = int.Parse(height.Remove(height.Length - 2, 2));
                if (!heightInCm.InRange(150, 193))
                {
                    return false;
                }
            }
            else if (height.EndsWith("in"))
            {
                int heightInIn = int.Parse(height.Remove(height.Length - 2, 2));
                if (!heightInIn.InRange(59, 76))
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            if (!Fields.ContainsKey("hcl"))
            {
                return false;
            }
            // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            string hairColour = Fields["hcl"];
            if ((hairColour.Length != 7) ||
                (hairColour[0] != '#') ||
                (hairColour.Skip(1).Where(
                    ch => ch.InRange('0', '9') ||
                          ch.InRange('a', 'f')
                ).Count() != 6))
            {
                return false;
            }

            if (!Fields.ContainsKey("ecl"))
            {
                return false;
            }
            // ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth
            string eyeColour = Fields["ecl"];
            if (!eyeColour.Equals("amb") &&
                !eyeColour.Equals("blu") &&
                !eyeColour.Equals("brn") &&
                !eyeColour.Equals("gry") &&
                !eyeColour.Equals("grn") &&
                !eyeColour.Equals("hzl") &&
                !eyeColour.Equals("oth"))
            {
                return false;
            }

            if (!Fields.ContainsKey("pid"))
            {
                return false;
            }
            // pid (Passport ID) - a nine-digit number, including leading zeroes
            string passportId = Fields["pid"];
            if ((passportId.Length != 9) ||
                (passportId.Where(ch => ch.InRange('0', '9')).Count() != 9))
            {
                return false;
            }

            return true;
        }

        public static IEnumerable<Passport> Parse(string s)
        {
            List<Passport> passports = new List<Passport>();

            var lines = s.SplitOnNewLine(false);

            Dictionary<string, string> fields = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                if (line.Length == 0)
                {
                    if (fields.Count == 0)
                    {
                        continue;
                    }

                    passports.Add(new Passport { Fields = fields });
                    fields = new Dictionary<string, string>();
                }
                else
                {
                    foreach (var kv in line.Split(" ")) {
                        var input = kv.Split(":");
                        Debug.Assert(input.Length == 2);
                        fields.Add(input[0], input[1]);
                    }
                }
            }

            if (fields.Count != 0)
            {
                passports.Add(new Passport { Fields = fields });
            }

            return passports;
        }
    }
    
    int part1(IEnumerable<Passport> inputs)
    {
        return inputs.Where(passport => 
            passport.Fields.ContainsKey("byr") &&
            passport.Fields.ContainsKey("iyr") &&
            passport.Fields.ContainsKey("eyr") &&
            passport.Fields.ContainsKey("hgt") &&
            passport.Fields.ContainsKey("hcl") &&
            passport.Fields.ContainsKey("ecl") &&
            passport.Fields.ContainsKey("pid")).Count();
    }

    int part2(IEnumerable<Passport> inputs)
    {
        return inputs.Where(passport => passport.IsValid()).Count();
    }

    public void run()
    {
        var input = Passport.Parse(File.ReadAllText(@"input/day04.txt"));

        var testInput1 = Passport.Parse(@"
ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in");

        Debug.Assert(part1(testInput1) == 2);

        var testInput2 = Passport.Parse(@"
eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007");
        var testInput3 = Passport.Parse(@"
pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719");

        Debug.Assert(part2(testInput2) == 0);
        Debug.Assert(part2(testInput3) == 4);

        Console.WriteLine($"Part 1: {part1(input)}");
        Console.WriteLine($"Part 2: {part2(input)}");
    }
}