import React from 'react';
import s from './Loading.module.css';
import {Spin} from "antd";

export const Loading: React.FC = () => {
    return (
        <div className={s.wrapper_svg}>
            <Spin size={'large'}/>
        </div>
    );
}
