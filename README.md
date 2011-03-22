#Dynamic REST

This is a small project demonstrating REST service access,
working against JSON data and XML data using late-bound
dynamic code using the new dynamic programming features being
added to c# 4.0.

JSON data
http://www.nikhilk.net/CSharp-Dynamic-Programming-JSON.aspx

REST client
http://www.nikhilk.net/CSharp-Dynamic-Programming-REST-Services.aspx

## Basic usage

* To get an instance of RestClient use the RestClientBuilder fluent interface:

        var client = new RestClientBuilder()
                .WithAcceptHeader("application/json")
                .WithUri("http://some.uri")
                .WithOAuth2Token("token")
                .Build();
ss
* Issue a GET

        var response = client.Get();
        
* Navigate the response
  
  Given this response:
  
        {
          article:{
            images:[
              { src:'http://some.uri/image1.png' },
              { src:'http://some.uri/image2.png' }
            ]
          }
        }
        
  You can navigate using the following dynamic syntax:
  
        var image2src = response.Result.article.images[1].src;
        
