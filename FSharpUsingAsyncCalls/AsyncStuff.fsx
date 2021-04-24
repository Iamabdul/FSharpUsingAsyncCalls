(*
async { <- computational expression
        !               ->              Bang
        !               ->              always work with async
        no !            ->              no async

        let x = functionOrFunctionCallOrValue            ->             normal value binding
        let! x = functionOrFunctionCallOrValueASYNC      ->             awaits the async Operation to complete

        use x = functionOrFunctionCallOrValue            ->             normal value binding with automatic disposal
        use! x = functionOrFunctionCallOrValueASYNC      ->             awaits the async operation to bind with automatic disposal

        do              ->              calls operation that returns the unit
        do!             ->              awaits the called operation to return the unit

        match functionOrFunctionCallOrValue with         ->             normal match
        match! functionOrFunctionCallOrValueASYNC with   ->             only after the async operation finishes that it commences matching

        return          ->              if this return is in an async block, it wraps the normal value in an Async object example: async {return 22} -> Async<int>
        return!         ->              returns an async without wrapping it: e.g async {return! functionOrFunctionCallOrValueASYNC } -> Async<int>.
                                        if we didnt include the !, the output would wrap the already wrapped async function so it would look like this: Async<Async<int>>
   }
*)
