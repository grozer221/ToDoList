import React, {FC} from 'react';
import {Link} from 'react-router-dom';
import {Button, Result} from 'antd';

type Props = {
    statusCode?: number,
}

export const Error: FC<Props> = ({statusCode}) => {
    switch (statusCode) {
        case 403:
            return (
                <Result
                    status="403"
                    title="403"
                    subTitle="Доступ заборонено."
                    extra={
                        <Link to={'/'}>
                            <Button type="primary">На гловну</Button>
                        </Link>
                    }
                />
            );
        default:
            return (
                <Result
                    status="404"
                    title="404"
                    subTitle="Сторінки не існує."
                    extra={
                        <Link to={'/'}>
                            <Button type="primary">На гловну</Button>
                        </Link>
                    }
                />
            );
    }

};
