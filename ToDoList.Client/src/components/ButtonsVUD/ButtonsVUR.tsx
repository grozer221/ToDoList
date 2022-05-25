import React, {FC} from 'react';
import {Link} from 'react-router-dom';
import {Avatar, Popconfirm, Tooltip} from 'antd';
import {DeleteOutlined, EyeOutlined, FormOutlined} from '@ant-design/icons';
import s from './ButtonsVUR.module.css';

type Props = {
    viewUrlA?: string,
    viewUrl?: string,
    onView?: () => void,
    updateUrl?: string,
    onUpdate?: () => void,
    removeUrl?: string,
    onRemove?: () => void,
}

export const ButtonsVUR: FC<Props> = ({viewUrlA, viewUrl, updateUrl, removeUrl, onView, onUpdate, onRemove}) => {
    return (
        <>
            <div className={s.buttonsVUR}>
                {onView ?
                    <Tooltip title="View">
                        <div className={s.buttonView} onClick={onView}>
                            <Avatar size={28} icon={<EyeOutlined/>}/>
                        </div>
                    </Tooltip>
                    : viewUrlA
                        ? <Tooltip title="View">
                            <a href={viewUrlA} target={'blank'} className={s.buttonView}>
                                <Avatar size={28} icon={<EyeOutlined/>}/>
                            </a>
                        </Tooltip>
                        : viewUrl &&
                        <Tooltip title="View">
                            <Link to={viewUrl} className={s.buttonView}>
                                <Avatar size={28} icon={<EyeOutlined/>}/>
                            </Link>
                        </Tooltip>
                }
                {onUpdate
                    ? <Tooltip title="Update">
                        <div className={s.buttonUpdate} onClick={onUpdate}>
                            <Avatar size={28} icon={<FormOutlined/>}/>
                        </div>
                    </Tooltip>
                    : updateUrl &&
                    <Tooltip title="Update">
                        <Link to={updateUrl} className={s.buttonUpdate}>
                            <Avatar size={28} icon={<FormOutlined/>}/>
                        </Link>
                    </Tooltip>
                }
                {onRemove
                    ? <Tooltip title="Remove">
                        <Popconfirm
                            title="Are you sure you want to delete?"
                            onConfirm={onRemove}
                            okText="Yes"
                            cancelText="No"
                        >
                            <div className={s.buttonRemove}>
                                <Avatar size={28} icon={<DeleteOutlined/>}/>
                            </div>
                        </Popconfirm>
                    </Tooltip>
                    : removeUrl &&
                    <Tooltip title="Remove">
                        <Link to={removeUrl} className={s.buttonRemove}>
                            <Avatar size={28} icon={<DeleteOutlined/>}/>
                        </Link>
                    </Tooltip>
                }
            </div>
        </>
    );
};
