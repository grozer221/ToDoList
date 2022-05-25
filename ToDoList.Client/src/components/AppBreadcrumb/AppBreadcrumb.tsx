import React, {FC} from 'react';
import {Breadcrumb} from 'antd';
import {useLocation} from 'react-router-dom';

export const AppBreadcrumb: FC = () => {
    const location = useLocation();
    let modules = location.pathname.split('/');
    modules = modules.filter(Boolean);

    return (
        <Breadcrumb>
            <Breadcrumb.Item key={'-1'}>.</Breadcrumb.Item>
            {modules.map((module, i) => <Breadcrumb.Item key={i}>{module}</Breadcrumb.Item>)}
        </Breadcrumb>
    );
};
