module Hynteract.Client.Main

open System.Net.Http
open Microsoft.AspNetCore.Components
open Elmish
open Bolero
open Bolero.Html
open Draw

/// Routing endpoints definition.
type Page =
    | [<EndPoint "/">] Home
    | [<EndPoint "/draw">] Draw

/// The Elmish application's model.
type Model =
    {
        page: Page
        MousePosition : int*int
    }

let initModel =
    {
        page = Home
        MousePosition = 0,0
    }

/// The Elmish application's update messages.
type Message =
    | SetPage of Page
    | MouseMoved of int*int


let update (http: HttpClient) message model =
    match message with
    | SetPage page ->
        { model with page = page }, Cmd.none

    | MouseMoved (x, y) -> { model with MousePosition = (x, y) }, Cmd.none



/// Connects the routing system to the Elmish application.
let router = Router.infer SetPage (fun model -> model.page)

type Main = Template<"wwwroot/main.html">

let homePage model dispatch =
    Main.Home().Elt()


let drawPage model dispatch =
    Main.Draw().Elt()
    (*div{
        div {
            // Styling to create a visible rectangular space
            attr.``style`` $"position:relative;
                            width:{rectWidth}px;
                            height:{rectHeight}px;
                            border: 2px solid black;
                            margin:100px"

            // Add mousemove event listener restricted to the rectangular space
            on.mousedown (fun e ->
                let x = int e.ClientX - 374
                let y = int e.ClientY - rectTop - 100
                if x >= 0 && y >= 0 && x <= rectWidth+rectLeft && y <= rectHeight+rectTop then
                    dispatch (MouseMoved (x, y))
                else
                    () // Do nothing if outside the rectangular space
            )

            // Display the current mouse position within the rectangular space
            p {
                textf "Mouse Position: %d, %d" (fst model.MousePosition) (snd model.MousePosition)
            }
        }

    }*)



let menuItem (model: Model) (page: Page) (text: string) =
    Main.MenuItem()
        .Active(if model.page = page then "is-active" else "")
        .Url(router.Link page)
        .Text(text)
        .Elt()

let view model dispatch =
    Main()
        .Menu(concat {
            menuItem model Home "Home"
            menuItem model Draw "Draw"
        })
        .Body(
            cond model.page <| function
            | Home -> homePage model dispatch
            | Draw -> drawPage model dispatch
        )

        
        .Elt()

type MyApp() =
    inherit ProgramComponent<Model, Message>()
    [<Inject>]
    member val HttpClient = Unchecked.defaultof<HttpClient> with get, set

    override this.Program =
        let update = update this.HttpClient
        Program.mkProgram (fun _ -> initModel, Cmd.none) update view
        |> Program.withRouter router
