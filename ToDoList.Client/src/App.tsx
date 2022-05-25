import React from 'react';
import {Navigate, Route, Routes} from "react-router-dom";
import {ToDosIndex} from "./pages/todos/ToDosIndex/ToDosIndex";
import {ToDosCreate} from "./pages/todos/ToDosCreate/ToDosCreate";
import {AppLayout} from "./components/AppLayout/AppLayout";
import './App.css';
import 'antd/dist/antd.css';

export const App = () => {
    return (
        <AppLayout>
            <Routes>
                <Route index element={<Navigate to={'/todos'}/>}/>
                <Route path={'todos/*'}>
                    <Route index element={<ToDosIndex/>}/>
                    <Route path="create" element={<ToDosCreate/>}/>
                    <Route path={'*'} element={<h1>Not Found todos</h1>}/>
                </Route>
                <Route path={'*'} element={<h1>Not Found</h1>}/>
            </Routes>
        </AppLayout>
    );
}