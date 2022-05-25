import React, {FC, useEffect, useState} from 'react';
import {Layout, Menu} from 'antd';
import s from './AppLayout.module.css';
import {AppBreadcrumb} from "../AppBreadcrumb/AppBreadcrumb";
import {Link, useLocation} from "react-router-dom";

const {Header, Content, Footer} = Layout;

type Props = {
    children?: React.ReactNode
}


export const AppLayout: FC<Props> = ({children}) => {
    const location = useLocation();
    const [defaultSelectedKey, setDefaultSelectedKey] = useState('');

    useEffect(() => {
        const newDefaultSelectedKey = getDefaultSelectedKey(location.pathname);
        if (newDefaultSelectedKey !== defaultSelectedKey)
            setDefaultSelectedKey(newDefaultSelectedKey);
    }, [location.pathname]);

    const getDefaultSelectedKey = (path: string): string => {
        if (path.match(/todos/i))
            return 'todos';
        else if (path.match(/categories/i))
            return 'categories';
        else
            return '';
    }

    console.log(defaultSelectedKey)

    return (
        <Layout className={s.layout}>
            <Header>
                <div className={s.logo}/>
                <Menu theme="dark" mode="horizontal" defaultSelectedKeys={[defaultSelectedKey]}>
                    <Menu.Item key={'todos'}>
                        <Link to={'/todos'}>
                            Todos
                        </Link>
                    </Menu.Item>
                    <Menu.Item key={'categories'}>
                        <Link to={'/categories'}>
                            Categories
                        </Link>
                    </Menu.Item>
                </Menu>
            </Header>
            <Content className={s.content}>
                <div className={s.appBreadcrumb}>
                    <AppBreadcrumb/>
                </div>
                <div className={s.siteLayoutContent}>{children}</div>
            </Content>
        </Layout>
    );
};