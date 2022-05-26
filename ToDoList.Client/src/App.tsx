import React from 'react';
import {Navigate, Route, Routes} from "react-router-dom";
import {TodosIndex} from "./pages/todos/TodosIndex/TodosIndex";
import {TodosCreate} from "./pages/todos/TodosCreate/TodosCreate";
import {AppLayout} from "./components/AppLayout/AppLayout";
import './App.css';
import 'antd/dist/antd.css';
import {TodosUpdate} from "./pages/todos/TodosUpdate/TodosUpdate";

export const App = () => {
    return (
        <AppLayout>
            <Routes>
                <Route index element={<Navigate to={'/todos'}/>}/>
                <Route path={'todos/*'}>
                    <Route index element={<TodosIndex/>}/>
                    <Route path="create" element={<TodosCreate/>}/>
                    <Route path="update/:id" element={<TodosUpdate/>}/>
                    <Route path={'*'} element={<h1>Not Found todos</h1>}/>
                </Route>
                <Route path={'*'} element={<h1>Not Found</h1>}/>
            </Routes>
        </AppLayout>
    );
}