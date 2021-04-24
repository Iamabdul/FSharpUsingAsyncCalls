// Learn more about F# at http://fsharp.org

open System.Net
open System.IO


let sites = [
    "http://www.bing.com";
    "http://www.google.com";
    "http://www.microsoft.com";
    "http://www.amazon.com";
    "http://www.yahoo.com"
]

let fetchUrl url = 
    let request = WebRequest.Create(System.Uri url)
    use response = request.GetResponse()
    use stream = response.GetResponseStream()
    use reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading %s" url


//the list map function here is looping through the string list of sites and applying each item as a parameter to the fetchUrl function
//this means in c# (foreach var siteUrl in sites) { fetchUrl(siteUrl) ;} but it does it in a simpler way
//the "|> ignore" keyword is just an expression to say "whatever result this thing pipes to, ignore it"
//the f# intellisense senses that these functions return units and so are in "danger" of being ignored, hence the worning
#time //turn interactive timer on
sites |> List.map fetchUrl |> ignore //do the thing for the sites (ignore outcome)
#time //stop timer


//this function effectively tries to download all of them ONE BY ONE
//Maybe we'd like to download them in parallel, and that is our goal here
//This function is similar to the one previously created, but the biggest change here is the "async" keyword,
//one question I havent been able to find out yet is...does this async keyword in f# also create a statemachine as soon as it's used (like c#)?
//the function signature for this function also differs to the previous in that it wraps the unit in an async object
//what this does is it provides us with an object that can be run asynchronosly later if we decide to
//NOTE: It provides the async object wrapping a unit, but it doesnt actually execute the function
let fetchUrlAsync url =
  async {
    let request = WebRequest.Create(System.Uri url)
    use! response = request.AsyncGetResponse() /// Here we can see that the "use" now has an additional "!" next to it, I think this is called a "bang" so we would say "useBang" 
    //we also changed that line from "GetResponse" to "AsyncGetResponse"
    //What the "use!" is telling the compiler is that it expects an async wrapped object (most likely WebRespnse in this context) so it allows us to wait for response
    //without blocking the current thread, the OS is now able to go to another part of the process to check what else needs doing while this finishes up (same as c#)
    use stream = response.GetResponseStream()
    use reader = new StreamReader(stream)
    let html = reader.ReadToEnd()
    printfn "finished downloading %s" url
    }

//Acouple of things have change in this method
#time //turn interactive timer on
sites |> List.map fetchUrlAsync //makes a list of async tasks
|> Async.Parallel //sets up the tsks to run in parallel
|> Async.RunSynchronously //runs the async computation and "awaits" its result
|> ignore //do the thing for the sites (ignore outcome)
#time //stop timer