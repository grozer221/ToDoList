import React from 'react';
import {Link} from "react-router-dom";
import {useDispatch} from "react-redux";
import {todosActions} from "../../../redux/todos/actions";

export const ToDosCreate = () => {
    const dispatch = useDispatch();

    return (
        <div>
            <h1>ToDosCreate</h1>
            <Link to={'/'}>home</Link>
        </div>
    );
};