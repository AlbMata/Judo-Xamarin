
The judoPay library lets you integrate card payments into your Xamarin project. It is built to be mobile first with ease of integration in mind. Judo's SDK enables a faster, simpler and more secure payment experience within your app. Build trust and user loyalty in your app with our secure and intuitive UX.

# Features

- **Simplicity** - Our simple integration process means you can have more time to develop your project and leave the intricacy of payments to us..
- **Security** - Use our out-of-the-box UI to enjoy in-built PCI Level 1 compliance, so all your transactions are handled in the safest hands.
- **Flexibility** - Integrate with our full suite of payment API's for complete control of your application UX. 

# Learn more

Learn more about judo by visiting [judopay.com](https://www.judopay.com/xamarin?utm_source=xamarin&utm_medium=partnership%20inbound%20links&utm_term=xamarin%20components%20%2D%20learn%20more%20link&utm_content=partnerships&utm_campaign=xamarin%20components%20%2D%20learn%20more%20link)

Or take a look at the code yourself [Github](https://github.com/JudoPay/Judo-Xamarin)

# Release 

Version 2.4.0

- Bug fixes
- Fraud improvements
- Apple Pay updates

# Notes 

Breaking change, when using non UI calls to the 
API you must recycle the session
Judo.Instance.CycleSession();
before making further calls.
This is to insure unique payment reference