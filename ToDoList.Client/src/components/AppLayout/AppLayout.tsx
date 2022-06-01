import React, {FC, useEffect, useState} from 'react';
import {Col, Layout, Menu, Row, Select} from 'antd';
import s from './AppLayout.module.css';
import {AppBreadcrumb} from "../AppBreadcrumb/AppBreadcrumb";
import {Link, useLocation} from "react-router-dom";
import {DataProvider} from "../../graphQL/enums/dataProvider";
import Cookies from 'js-cookie';

const {Header, Content} = Layout;

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

    const changeDataProviderHandler = (value: keyof typeof DataProvider) => {
        Cookies.set('DataProvider', value);
        window.location.reload();
    }

    return (
        <Layout className={s.layout}>
            <Header>
                <Row >
                    <Col span={22   }>
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
                    </Col>
                    <Col span={2}>
                        <Select defaultValue={Cookies.get('DataProvider') as DataProvider || DataProvider.Database} onChange={changeDataProviderHandler} style={{width: '100%'}}>
                            {(Object.keys(DataProvider) as Array<keyof typeof DataProvider>).map(dataProvider => (
                                <Select.Option key={dataProvider} value={dataProvider}>{dataProvider}</Select.Option>
                            ))}
                        </Select>
                    </Col>
                </Row>
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