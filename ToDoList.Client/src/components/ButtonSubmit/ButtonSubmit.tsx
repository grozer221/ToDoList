import React, {FC} from 'react';
import {Button} from 'antd';

type Props = {
    children?: React.ReactNode,
    loading?: boolean | undefined,
    isSubmit?: boolean | undefined,
};

export const ButtonSubmit: FC<Props> = ({loading, isSubmit, children}) => {
    return (
        <Button loading={loading} type={'primary'} htmlType={!!isSubmit ? 'submit' : 'button'}>{children}</Button>
    );
};
