// src/components/organisms/HeaderAvaliacao.jsx
import React from "react";
import { Card, Tag, Button, Space, Descriptions, Popconfirm } from "antd";
import { PauseCircleOutlined, ReloadOutlined, CheckCircleOutlined } from "@ant-design/icons";

const statusMap = {
  1: { color: "blue", label: "Em aberto" },
  2: { color: "orange", label: "Interrompida" },
  3: { color: "green", label: "Finalizada" },
};

const HeaderAvaliacao = ({ avaliacao, onInterromper, onReativar, onFinalizar }) => {
  const { id, animalId, linhagem, dataInicioAvaliacao, statusAvaliacao } = avaliacao;
  const numericStatus = Number(statusAvaliacao) || 1;
  const statusInfo = statusMap[numericStatus] || { color: "default", label: "Desconhecido" };

  const formatarData = (data) =>
    new Date(data).toLocaleDateString("pt-BR", { day: "2-digit", month: "2-digit", year: "numeric" });

  return (
    <Card title={`Avaliação #${id}`} extra={<Tag color={statusInfo.color}>{statusInfo.label}</Tag>} className="shadow-md rounded-xl">
      <Descriptions column={2} size="small" bordered>
        <Descriptions.Item label="Linhagem">{linhagem || "-"}</Descriptions.Item>
        <Descriptions.Item label="Animal ID">{animalId ?? "-"}</Descriptions.Item>
        <Descriptions.Item label="Data de Início">{formatarData(dataInicioAvaliacao)}</Descriptions.Item>
        <Descriptions.Item label="Status">{statusInfo.label}</Descriptions.Item>
      </Descriptions>

      <div className="mt-4 flex justify-end">
        <Space>
          {numericStatus === 1 && (
            <Popconfirm title="Interromper avaliação" description="Tem certeza?" onConfirm={onInterromper}>
              <Button type="primary" danger icon={<PauseCircleOutlined />}>Interromper</Button>
            </Popconfirm>
          )}

          {numericStatus === 2 && (
            <Popconfirm title="Reativar avaliação" description="Tem certeza?" onConfirm={onReativar}>
              <Button type="default" icon={<ReloadOutlined />}>Reativar</Button>
            </Popconfirm>
          )}

          {numericStatus === 1 && (
            <Popconfirm title="Finalizar avaliação" description="Confirma finalização?" onConfirm={onFinalizar}>
              <Button type="primary" icon={<CheckCircleOutlined />}>Finalizar</Button>
            </Popconfirm>
          )}
        </Space>
      </div>
    </Card>
  );
};

export default HeaderAvaliacao;
