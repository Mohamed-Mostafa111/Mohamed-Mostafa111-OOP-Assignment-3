using StaticMethods;
Console.WriteLine("=== Phone Number Test ===");

Console.WriteLine("Is valid Egyptian phone number: {0}", "01012345678".IsvalidEgyptionPhoneNumber());
Console.WriteLine("Is valid Egyptian phone number: {0}", "+201512345678".IsvalidEgyptionPhoneNumber());
Console.WriteLine("Is valid Egyptian phone number: {0}", "01312345678".IsvalidEgyptionPhoneNumber());
Console.WriteLine("Is valid Egyptian phone number: {0}", "0101234567".IsvalidEgyptionPhoneNumber());
Console.WriteLine("Is valid Egyptian phone number: {0}", "0101234567a".IsvalidEgyptionPhoneNumber());

Console.WriteLine("=== National Id Test ===");
Console.WriteLine("Is valid Egyptian national ID: {0}", "29901011234567".IsvalidEgyptionNationalId());
Console.WriteLine("Is valid Egyptian national ID: {0}", "19901011234567".IsvalidEgyptionNationalId());
Console.WriteLine("Is valid Egyptian national ID: {0}", "2990101123456".IsvalidEgyptionNationalId());