import React from 'react';
import ReactDOM from 'react-dom/client';
import {App} from './App';
import {BrowserRouter} from "react-router-dom";
import {Provider} from "react-redux";
import {store} from "./store/store";
import enUS from 'antd/lib/locale/en_US';
import {ConfigProvider} from "antd";

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <React.StrictMode>
        <BrowserRouter>
            <Provider store={store}>
                <ConfigProvider locale={enUS}>
                    <App/>
                </ConfigProvider>
            </Provider>
        </BrowserRouter>
    </React.StrictMode>
);