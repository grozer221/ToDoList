import React from 'react';
import {Navigate, Route, Routes} from "react-router-dom";
import {TodosIndex} from "./pages/todos/TodosIndex/TodosIndex";
import {TodosCreate} from "./pages/todos/TodosCreate/TodosCreate";
import {AppLayout} from "./components/AppLayout/AppLayout";
import './App.css';
import 'antd/dist/antd.css';
import {TodosUpdate} from "./pages/todos/TodosUpdate/TodosUpdate";
import {CategoriesIndex} from "./pages/categories/CategoriesIndex/CategoriesIndex";
import {CategoriesCreate} from "./pages/categories/CategoriesCreate/CategoriesCreate";
import {CategoriesUpdate} from "./pages/categories/CategoriesUpdate/CategoriesUpdate";
import {Error} from "./components/Error/Error";

export const App = () => {
    return (
        <AppLayout>
            <Routes>
                <Route index element={<Navigate to={'/todos'}/>}/>
                <Route path={'todos/*'}>
                    <Route index element={<TodosIndex/>}/>
                    <Route path="create" element={<TodosCreate/>}/>
                    <Route path="update/:id" element={<TodosUpdate/>}/>
                    <Route path={'*'} element={<Error/>}/>
                </Route>
                <Route path={'categories/*'}>
                    <Route index element={<CategoriesIndex/>}/>
                    <Route path="create" element={<CategoriesCreate/>}/>
                    <Route path="update/:id" element={<CategoriesUpdate/>}/>
                    <Route path={'*'} element={<Error/>}/>
                </Route>
                <Route path={'*'} element={<Error/>}/>
            </Routes>
        </AppLayout>
    );
}